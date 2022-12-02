// возвращает куки с указанным name,
// или undefined, если ничего не найдено
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function showHide() {
    let cookie = getCookie("session-id")
    if (cookie !== undefined) {
        let obj = document.getElementById("signin_block");
        obj.style.display = "none"; //Скрываем элемент
    }
    else
    {
        let obj = document.getElementById("profile_block");
        obj.style.display = "none"; 
    }
    
}   