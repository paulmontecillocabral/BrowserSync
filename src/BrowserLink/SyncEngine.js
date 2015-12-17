/// <reference path="../_intellisense/browserlink.intellisense.js" />

(function (browserLink, $) {
    /// <param name="browserLink" value="bl" />
    /// <param name="$" value="jQuery" />

    function syncNavigation(url, xpos, ypos) {
        if (location.href !== url) {
            location.href = url;
        }
        else {
            window.scrollTo(xpos, ypos);
        }
    }

    function syncForm(selector, value) {
        console.log("recieved: " + value);
        var element = $(selector);
        var tagName = element[0].tagName;

        if (tagName === "INPUT" || tagName === "TEXTAREA") {
            element.val(value);
        } else {
            element.html(value);
        }
    }

    function connectionHandler() {

        document.onkeydown = function (e) {
            var evt = e || event;

            if (evt.altKey && evt.ctrlKey && evt.keyCode === 13) {
                evt.preventDefault();
                browserLink.invoke("Navigate", window.pageXOffset, window.pageYOffset);
            }
        };

        $("body").on("input blur", "input, textarea, [contenteditable]", function () {
            var self = $(this);
            var id = self.attr("id");
            var name = self.attr("name");
            var selector;

            if (id) {
                selector = "#" + id;
            } else if (name) {
                selector = self[0].tagName + "[name='" + name + "']"
            }

            if (selector) {
                var value = self.val() || self.html();
                browserLink.invoke("FormSync", selector, value);
            }
        });
    }

    return {

        onConnected: connectionHandler, // Fires automatically when Browser Link connects
        syncNavigate: syncNavigation,
        syncForm: syncForm
    };
});