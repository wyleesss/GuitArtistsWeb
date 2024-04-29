document.addEventListener("DOMContentLoaded", function () {
    const userContainer = document.querySelector(".user-info-container");
    const userPanel = document.createElement("div");
    userPanel.classList.add("user-panel");

    userPanel.innerHTML = `
    <div class="text-center">
        <br>
        <img style="margin-bottom: 15px" src="${userContainer.dataset.photo}" alt="logo" class="user-info-logo"><br>
        <a style="color: white; font-size: 20px"><em><b>${userContainer.dataset.username}</b></em></a><br><hr>
        <a><b>Контент</b></a><br><br>
        <a href="/articles" style="color: lightgray">Статті</a><br><br>
        <a href="/lessons" style="color: lightgray">Уроки</a><br><br>
        <a href="/chords" style="color: lightgray">Акорди</a><br><br><br>
        <a><b>Творчість</b></a><br><br>
        <a href="#" style="color: lightgray">Створити статтю</a><br><br><br>
        <a><b>Аккаунт</b></a><br><br>
        <a href="#" style="color: lightgray">Профіль</a><br><br>
        <a href="#" style="color: red">Вийти</a>
    </div>
    `;

    userContainer.appendChild(userPanel);

    const overlay = document.createElement("div");
    overlay.classList.add("overlay");
    document.body.appendChild(overlay);

    userContainer.addEventListener("mouseenter", function () {
        userPanel.style.display = "block";
        overlay.style.display = "block";
    });

    userContainer.addEventListener("mouseleave", function () {
        userPanel.style.animation = "slideOut 0.3s forwards";
        overlay.style.display = "none";
        setTimeout(() => {
            userPanel.style.display = "none";
            userPanel.style.animation = "slideIn 0.3s forwards";
        }, 300);
    });

    userContainer.addEventListener("mouseenter", function () {
        userPanel.classList.add("show");
    });

    userContainer.addEventListener("mouseleave", function () {
        userPanel.classList.remove("show");
    });
});
