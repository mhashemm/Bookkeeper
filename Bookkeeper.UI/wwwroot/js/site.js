// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll("time").forEach((el) => {
	el.textContent = new Date(Date.parse(el.dateTime)).toLocaleString("en-AU");
});
