function changeColorRed(element) {
    element.style.border = "2px solid red";
}
function changeColorGreen(element) {
    element.style.border = "2px solid green";
}

// Определяем функции для отображения сообщения об ошибке
function printError(elemId, hintMsg) {
    document.getElementById(elemId).innerHTML = hintMsg;
}

// Определяем функции для проверки формы
function validateForm() {
    // Получение значений элементов формы
    var login = document.registration_form.login.value;
    var password = document.registration_form.password.value;
    var loginErr = passErr = true;

    if(login == "") {
        changeColorRed(document.getElementById("login"))
        printError("loginErr", "Пожалуйста, введите Логин");
    } else {
        var regex = /[a - zA - Z0 - 9] +@[a - zA - Z0 - 9]+\.[a - zA - Z0 - 9]/;
        if(regex.test(login) === false) {
            changeColorRed(document.getElementById("login"))
            printError("loginErr", "Введите корректный e-mail");
        } else {
                changeColorGreen(document.getElementById("login"))
                printError("loginErr", "");
                loginErr = false;
            }
        }
    }

    if(password == "") {
        changeColorRed(document.getElementById("password"))
        printError("passErr", "Пожалуйста, введите Пароль");
    } else {
        var regex = /(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/;
        if(regex.test(password) === false) {
            changeColorRed(document.getElementById("password"))
            printError("passErr", "Строчные и прописные латинские буквы, цифры, спецсимволы. Минимум 8 символов");
        } else{
            changeColorGreen(document.getElementById("password"))
            printError("passErr", "");
            passErr = false;
        }
    }
    
    if((loginErr || passErr) == true) {
       return false;
    } else {
        alert("Регистрация прошла успешно!");
        document.getElementById("register").disabled = false
        document.getElementById("register").click()
        document.getElementById("register").disabled = true
    }
};