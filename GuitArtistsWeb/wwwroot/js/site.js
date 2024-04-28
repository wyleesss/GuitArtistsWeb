document.addEventListener("DOMContentLoaded", function () {
    const userContainer = document.querySelector(".user-info-container");
    const userPanel = document.createElement("div");
    userPanel.classList.add("user-panel");

    userPanel.innerHTML = `
        <a href="#">Посилання 1</a>
        <a href="#">Посилання 2</a>
        <a href="#">Посилання 3</a>
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
