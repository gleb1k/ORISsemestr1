// возвращает куки с указанным name,
// или undefined, если ничего не найдено
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function addAnimeOrShowError() {
    let cookie = getCookie("session-id")
    let btnAdd = document.getElementById("btn_add_anime");
    let authText = document.getElementById("auth_to_add_text");
    if (cookie === undefined) {
        btnAdd.style.display = "none";
        authText.style.display = "block";
    } else {
        btnAdd.style.display = "block";
        authText.style.display = "none";
    }

}   