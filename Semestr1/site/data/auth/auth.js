function printError(elemId, hintMsg) {
    document.getElementById(elemId).innerHTML = hintMsg;
}

function changeColorRed(element) {
    element.style.border = "2px solid red";
}

function changeColorGreen(element) {
    element.style.border = "2px solid green";
}

function validate() {
    let login = document.getElementById("login");
    let loginValue = document.getElementById("login").value;
    let password = document.getElementById("password");
    let passwordValue = document.getElementById("password").value;

    let loginRegular = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    //длина пароля минимум 4 символа
    let passwordRegular = /(?=.*[0-9])(?=.*[a-z])[0-9a-zA-Z*]{4,}/g

    var loginErr = passErr = true;

    if (loginValue == "") {
        printError('loginErr', 'Необходимо ввести e-mail');
        changeColorRed(login)
        return;
    } else {
        if (loginRegular.test(loginValue) === false) {
            printError('loginErr', 'Введите корректный e-mail');
            changeColorRed(login)
            return;
        } else {
            if (loginValue.length > 50) {
                printError('loginErr', 'Недопустимый размер e-mail');
                changeColorRed(login)
                return;
            } else {
                printError('loginErr', '');
                changeColorGreen(login)
                loginErr = false;
            }
        }

        if (passwordValue == "") {
            printError("loginErr", "Необходимо ввести пароль");
            changeColorRed(password)
            return;
        } else {
            if (passwordRegular.test(passwordValue) === false) {
                printError("loginErr", "Пароль должен содержать прописные латинские буквы, цифры. Минимум 8 символов");
                changeColorRed(password)
                return;
            } else {
                if (passwordValue.length > 100) {
                    printError('loginErr', 'Недопустимый размер пароля');
                    changeColorRed(password)
                    return;
                } else {
                    printError("loginErr", "");
                    changeColorGreen(password)
                    passErr = false;
                }
            }
        }

        if (((loginErr || passErr) === true) || ((checkLength(loginValue) || checkLength(passwordValue)) === false)) {
            return false;
        } else {
            //валидация прошла успешно
            return true;
        }
    }
}
