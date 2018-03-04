document.addEventListener('DOMContentLoaded', getRandomQuestion, false);

var urlBase = 'http://localhost:53418/api/'

function getRandomQuestion() {
    runXhttpReq(setNewQuestion, 'Question/Random', 'GET')
}

function setNewQuestion(questionData) {
    var form = document.forms["questionForm"];
    form["questionId"].value = questionData.QuestionId;
    form["questionText"].value = questionData.QuestionText;
    form["answer"].value = "";
}

function getWotd() {
    runXhttpReq(setWordOfTheDay, 'http://localhost:49541/api/wotd', 'GET')
    checkShowAllowSuggest()
}

function setWordOfTheDay(wotd) {
    var elem = document.getElementById("wotd")
    elem.innerText = wotd
}

function suggestWotd(newWotd) {
    runXhttpReq(suggestWotdCallback, 'http://localhost:49541/api/suggest?WordOfTheDay=' + newWotd, 'POST')
}

function suggestWotdCallback(response) {
    var res = JSON.parse(response)
    var coloration = res.success ? 'success' : 'error'
    var str = '<h3>Status: <span class="' + coloration + '">'
        + res.status + '</span></h3>' + res.comment
    var elem = document.getElementById("suggestion-result-box")
    elem.innerHTML = str

    if (hidden("suggestion-result-box")) {
        unHide("suggestion-result-box")
    }

    if (res.success) {
        unHide("burn-it-down")
    } else if (!hidden("burn-it-down")) {
        hide("burn-it-down")
    }

}

function checkShowAllowSuggest() {
    if (++presses == thingThatCouldBeConfigurable) { // right? that's how ++var works i think
        unHide("suggest-the-suggestion-box")
    }
}


function reset() {
    hide("burn-it-down")
    hide("suggestion-result-box")
    hide("suggestion-box")
    hide("suggest-the-suggestion-box")

    presses = 0
}


function allowSuggest() {
    unHide("suggestion-box")
}

function disallowSuggest() {
    hide("suggestion-box")
}

function hidden(id) {
    var elem = document.getElementById(id)
    return elem.classList.contains("invisible")
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
function runXhttpReq(callback, url, method, body) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (validateXhttp(this)) {
            callback(JSON.parse(xhttp.response))
        }
    }

    xhttp.open(method, urlBase + url, true)
    
    // send a little differently based on whether we want to include
    // a request body
    if (body !== undefined) {
        xhttp.setRequestHeader("Content-Type", "application/json")
        xhttp.send(JSON.stringify(body))
    } else {
        xhttp.send()
    }
}

function validateXhttp(xhttp) {
    return xhttp.readyState == 4 && xhttp.status == 200
}