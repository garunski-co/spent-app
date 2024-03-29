"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
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
var Link = /** @class */ (function () {
    function Link() {
    }
    Link.launchLink = function (linkToken) {
        return __awaiter(this, void 0, void 0, function () {
            var linkPromise, _a, _b;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        linkPromise = function () {
                            return new Promise(function (resolve) {
                                // Needs to match C# Shared.LinkResult class
                                var result = {
                                    success: false,
                                    publicToken: null,
                                    error: null,
                                    metadata: null
                                };
                                var handler = Plaid.create({
                                    token: linkToken,
                                    onLoad: function () {
                                        // Optional, called when Link loads
                                    },
                                    onSuccess: function (publicToken, metadata) {
                                        result.success = true;
                                        result.publicToken = publicToken;
                                        result.metadata = metadata;
                                        resolve(result);
                                    },
                                    onExit: function (error, metadata) {
                                        // The user exited the Link flow.
                                        result.error = error;
                                        result.metadata = metadata;
                                        resolve(result);
                                    },
                                    onEvent: function (eventName, metadata) {
                                        // Optionally capture Link flow events, streamed through
                                        // this callback as your users connect an Item to Plaid.
                                    }
                                });
                                handler.open();
                            });
                        };
                        _b = (_a = JSON).stringify;
                        return [4 /*yield*/, linkPromise()];
                    case 1: return [2 /*return*/, _b.apply(_a, [_c.sent()])];
                }
            });
        });
    };
    return Link;
}());
