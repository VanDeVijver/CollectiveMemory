// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Clips carousel: stop playback when a page is left, and load a page's videos when it is shown.
(function () {
    var carousel = document.getElementById("clipCarousel");
    if (!carousel) return;

    carousel.addEventListener("slide.bs.carousel", function (e) {
        var leaving = carousel.querySelectorAll(".carousel-item")[e.from];
        if (leaving) {
            leaving.querySelectorAll("video").forEach(function (v) { v.pause(); });
            // An embedded player can't be paused from here; reloading its address stops it.
            leaving.querySelectorAll("iframe").forEach(function (f) { f.src = f.src; });
        }
        e.relatedTarget.querySelectorAll('video[preload="none"]').forEach(function (v) {
            v.preload = "metadata";
            v.load();
        });
    });
})();
