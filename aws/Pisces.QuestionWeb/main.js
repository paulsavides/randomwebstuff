document.addEventListener('DOMContentLoaded', getRandomQuestion, false);

var urlBase = 'https://api.paulsavides.com/api/'

/***********************
        AUTH STUFF 
************************/
function onSignIn(googleUser) {
    var token = googleUser.getAuthResponse().id_token;
    runXhttpReq(signInSuccess, 'Auth', 'POST', {IdToken: token}, onSignOut);
}

function onSignOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(() => {
        let questionPrompt = document.getElementById("question-prompt");
        questionPrompt.innerText = "Can you answer this?";    
        hide("user-widget");
        hide("logout-button");
        unHide("google-signin");
    });
}

function signInSuccess(data) {
    let imgWidget = document.getElementById("user-image");
    imgWidget.src = data.PictureUrl;

    let questionPrompt = document.getElementById("question-prompt");
    questionPrompt.innerText = "Can you answer this " + data.GivenName + "?";

    hide("google-signin");
    unHide("user-widget");
    unHide("logout-button");
}

/***********************
      QUESTION STUFF 
************************/
function getRandomQuestion() {
    runXhttpReq(setNewQuestion, 'Question/Random', 'GET')
}

function setNewQuestion(questionData) {
    var form = document.forms["questionForm"];
    form["questionId"].value = questionData.QuestionId;
    form["questionText"].value = questionData.QuestionText;
    form["answer"].value = "";
}

function verifyAnswer() {
    var form = document.forms["questionForm"];
    let qid = form["questionId"].value;
    let answer = form["answer"].value;

    if (qid !== undefined && answer !== undefined && answer !== "") {
        runXhttpReq(checkAnswer, 'Question/' + qid + '/Answer?answer=' + answer, 'GET');
    }
}

function checkAnswer(answerData) {
    if (answerData.Correct) {
        alert('ya got it right');
    } else {
        alert('its an easy one think about it');
    }
}

function hide(id) {
    var elem = document.getElementById(id)
    elem.classList.add("invisible")
}

function unHide(id) {
    var elem = document.getElementById(id)
    elem.classList.remove("invisible")
}

// Helper functions for xhttp stuff
function runXhttpReq(callback, url, method, body, errorCallback) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (validateXhttp(this)) {
            callback(JSON.parse(xhttp.response))
        } else if (checkXhttpError(this)) {
            if (errorCallback !== undefined) {
                errorCallback()
            }
        }
    }

    xhttp.open(method, urlBase + url, true)
    xhttp.setRequestHeader("Content-Type", "application/json")
    
    // send a little differently based on whether we want to include
    // a request body
    if (body !== undefined) {
        xhttp.send(JSON.stringify(body))
    } else {
        xhttp.send()
    }
}

function validateXhttp(xhttp) {
    return xhttp.readyState === 4 && xhttp.status === 200
}

function checkXhttpError(xhttp) {
    return xhttp.readyState === 4 && xhttp.status !== 200
}