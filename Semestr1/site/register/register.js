function changeColorRed(element) {
    element.style.border = "2px solid red";
}

function changeColorGreen(element) {
    element.style.border = "2px solid green";
}

function printError(elemId, hintMsg) {
    document.getElementById(elemId).innerHTML = hintMsg;
}


function validateForm() {
    // Получение значений элементов формы
    var login = document.forms["registration_form"]["login"].value;
    var password = document.forms["registration_form"]["password"].value;
    var loginErr = passErr = true;

    if (login == "") {
        printError('loginErr', 'Необходимо ввести e-mail');
        changeColorRed(document.getElementById("login"))
    } else {
        var regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (regex.test(login) === false) {
            printError('loginErr', 'Введите корректный e-mail');
            changeColorRed(document.getElementById("login"))
        } else {
            printError('loginErr', '');
            changeColorGreen(document.getElementById("login"))
            loginErr = false;
        }
    }

    if (password == "") {
        printError("passErr", "Пожалуйста, введите Пароль");
        changeColorRed(document.getElementById("pass"))
    } else {
        var regex = /(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/;
        if (regex.test(password) === false) {
            printError("passErr", "Строчные и прописные латинские буквы, цифры, спецсимволы. Минимум 8 символов");
            changeColorRed(document.getElementById("pass"))
        } else {
            printError("passErr", "");
            changeColorGreen(document.getElementById("pass"))
            passErr = false;
        }
    }

    if ((loginErr || passErr) == true) {
        return false;
    } else {
        alert("Регистрация прошла успешно!");
    }
}