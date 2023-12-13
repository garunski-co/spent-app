class App {
    public static setCookie(name: string, value: string, seconds: number, remember: boolean, secure: boolean) {
        const date = new Date();
        date.setSeconds(date.getSeconds() + seconds);
        let cookie = `${name}=${value};path=/;samesite=strict;`;
        if (remember) {
            cookie = cookie += `expires=${date.toUTCString()};`;
        }
        if (secure) {
            cookie = `${cookie};secure`;
        }
        document.cookie = cookie;
    }

    public static removeCookie(name: string): void {
        document.cookie = `${name}=; Max-Age=0`;
    }

    public static goBack(): void {
        window.history.back();
    }

    /**
     * To disable the scrollbar of the body when showing the modal, so the modal can be always shown in the viewport without being scrolled out.
     **/
    public static setBodyOverflow(hidden: boolean) {
        document.body.style.overflow = hidden ? "hidden" : "auto";
    }

    public static applyBodyElementClasses(cssClasses: string[], cssVariables: any): void {
        cssClasses?.forEach(c => document.body.classList.add(c));
        Object.keys(cssVariables).forEach(key => document.body.style.setProperty(key, cssVariables[key]));
    }
}

declare class BitTheme {
    static init(options: any): void;
}

BitTheme.init({
    system: true,
    onChange: (newTheme: string) => {
        if (newTheme === 'dark') {
            document.body.classList.add('theme-dark');
            document.body.classList.remove('theme-light');
            document.querySelector("meta[name=theme-color]")!.setAttribute('content', '#0d1117');
        } else {
            document.body.classList.add('theme-light');
            document.body.classList.remove('theme-dark');
            document.querySelector("meta[name=theme-color]")!.setAttribute('content', '#ffffff');
        }
    }
});
