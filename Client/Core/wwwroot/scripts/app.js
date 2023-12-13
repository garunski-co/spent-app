"use strict";
var App = /** @class */ (function () {
    function App() {
    }
    App.setCookie = function (name, value, seconds, remember, secure) {
        var date = new Date();
        date.setSeconds(date.getSeconds() + seconds);
        var cookie = "".concat(name, "=").concat(value, ";path=/;samesite=strict;");
        if (remember) {
            cookie = cookie += "expires=".concat(date.toUTCString(), ";");
        }
        if (secure) {
            cookie = "".concat(cookie, ";secure");
        }
        document.cookie = cookie;
    };
    App.removeCookie = function (name) {
        document.cookie = "".concat(name, "=; Max-Age=0");
    };
    App.goBack = function () {
        window.history.back();
    };
    /**
     * To disable the scrollbar of the body when showing the modal, so the modal can be always shown in the viewport without being scrolled out.
     **/
    App.setBodyOverflow = function (hidden) {
        document.body.style.overflow = hidden ? "hidden" : "auto";
    };
    App.applyBodyElementClasses = function (cssClasses, cssVariables) {
        cssClasses === null || cssClasses === void 0 ? void 0 : cssClasses.forEach(function (c) { return document.body.classList.add(c); });
        Object.keys(cssVariables).forEach(function (key) { return document.body.style.setProperty(key, cssVariables[key]); });
    };
    return App;
}());
BitTheme.init({
    system: true,
    onChange: function (newTheme) {
        if (newTheme === 'dark') {
            document.body.classList.add('theme-dark');
            document.body.classList.remove('theme-light');
            document.querySelector("meta[name=theme-color]").setAttribute('content', '#0d1117');
        }
        else {
            document.body.classList.add('theme-light');
            document.body.classList.remove('theme-dark');
            document.querySelector("meta[name=theme-color]").setAttribute('content', '#ffffff');
        }
    }
});
