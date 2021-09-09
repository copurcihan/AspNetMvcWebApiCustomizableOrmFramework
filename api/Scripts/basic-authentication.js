(function () {
    if (!$('#input_username').length) {
        $(function () {
            var basicAuthUI =
                '<div class="input"><input placeholder="username" id="input_username" name="username" type="text" size="10"></div>' +
                '<div class="input"><input placeholder="password" id="input_password" name="password" type="password" size="10"></div>';
            $(basicAuthUI).insertBefore('#api_selector div.input:last-child');
            $("#input_apiKey").hide();
            $('#input_username').change(addAuthorization);
            $('#input_password').change(addAuthorization);
        });
        function addAuthorization() {
            var username = $('#input_username').val();
            var password = $('#input_password').val();
            if (username && username.trim() != "" && password && password.trim() != "") {
                var basicAuth = new SwaggerClient.PasswordAuthorization('basic', username, password);
                window.swaggerUi.api.clientAuthorizations.add("basicAuth", basicAuth);
                console.log("authorization added: username = " + username + ", password = " + password);
            }
        }
    }
})();

(function () {
    if (!$('#input_username').length) {

        $(function () {
            $('#input_apiKey').off();
            $('#input_apiKey').on('change', function () {
                var key = this.value;
                if (key && key.trim() !== '') {
                    swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("my-cool-api-key", key, "header"));
                }
            });
        });
    }
})();