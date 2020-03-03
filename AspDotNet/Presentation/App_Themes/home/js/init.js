$(function () {
    M.AutoInit();

    loadProgressBar();

    $("#btn-signin").on('click', function (e) {
        $(this).addClass("disabled");
        e.preventDefault();

        var data = $("#login-form").serialize();

        axios.post('/token', data)
            .then(function (response) {
                M.toast({html: 'Login successful!', classes: 'green darken-3' });
                localStorage.setItem("token", response.data.access_token);
                localStorage.setItem("client_id", response.data.client_id);
                localStorage.setItem("refresh_token", response.data.refresh_token);
                window.location.href = "/home/authorize?access_token=" + response.data.access_token;           
            })
            .catch(function () {
                M.toast({ html: 'Invalid username or password !!' });
                $("#btn-signin").removeClass("disabled");
            });
    });
})
