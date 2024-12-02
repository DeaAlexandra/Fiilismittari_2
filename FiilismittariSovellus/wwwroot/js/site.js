// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const buttons = document.querySelectorAll(".mood-button");
    buttons.forEach(button => {
        const moodValue = button.getAttribute("data-mood");
        const img = document.createElement("img");
        img.src = `/Pictures/Faces/Emoji_${moodValue}.png`;
        img.alt = `Emoji ${moodValue}`;
        button.appendChild(img);
    });
});