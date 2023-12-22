declare class Plaid {
    static create(options: any): any;
}

class Link {
    public static async launchLink(linkToken: any) {
        const linkPromise = () => {
            return new Promise(resolve => {
                // Needs to match C# Shared.LinkResult class
                let result = {
                    success: false,
                    publicToken: null,
                    error: null,
                    metadata: null
                };

                let handler = Plaid.create({
                    token: linkToken,
                    onLoad: function () {
                        // Optional, called when Link loads
                    },

                    onSuccess: function (publicToken: any, metadata: any) {
                        result.success = true;
                        result.publicToken = publicToken;
                        result.metadata = metadata;
                        resolve(result);
                    },

                    onExit: function (error: any, metadata: any) {
                        // The user exited the Link flow.
                        result.error = error;
                        result.metadata = metadata;
                        resolve(result);
                    },

                    onEvent: function (eventName: any, metadata: any) {
                        // Optionally capture Link flow events, streamed through
                        // this callback as your users connect an Item to Plaid.
                    }

                });

                handler.open();
            })
        }

        return JSON.stringify(await linkPromise());
    }
}