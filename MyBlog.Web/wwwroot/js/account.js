$(document).on('click', '.js-useraction', function () {
    try {
        $.ajax({
            type: "POST",
            url: 'Accounts/BlockLogout',
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {
                console.log('Ok');
            },
            error: function (xhr) {
                console.log(xhr);
                location.reload();
            }
        })
    }
    catch (e) {
        console.log(e);
    }

    //to prevent default form submit event
    return false;
});

$(document).ready(function () {
    console.log('ready');
    try {
        $.ajax({
            type: "POST",
            url: 'Accounts/BlockLogout',
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {
            },
            error: function (xhr) {
                console.log(xhr.responseText)
            }
        })
    }
    catch (e) {
        console.log(e);
    }

    //to prevent default form submit event
    return false;

});

returnRegisterView = () => {
    $.ajax({
        type: "GET",
        url: "/Accounts/Register",
        success: function (res) {
            $("#loginModal").html(res);
        }
    })
};

returnLoginView = () => {
    $.ajax({
        type: "GET",
        url: "/Accounts/Login",
        success: function (res) {
            $("#loginModal").html(res);
        }
    })
};

$(document).on('click', '#loginViewBtn', function () {
    $('#loginModal').load("/Accounts/Login");
});

$(document).on('keyup', '.js-userRegex', function () {
    var node = $(this);
    node.val(node.val().replace(/[^a-zA-Z0-9]/g, ''));
});

$(document).on('keyup', '.loginfield input', function () {
    $.validator.unobtrusive.parse($('#loginForm'));
    console.log('login' + $('#loginForm').valid());
    let empty = false;
    $('.loginfield input').each(function () {
        empty = $(this).val().length == 0;
    });
    if (empty || !$('#loginForm').valid())
        $('.loginactions input').attr('disabled', 'disabled');
    else {
        $('.loginactions input').attr('disabled', false);
        var container = $('#loginForm').find('[data-valmsg-summary="true"]');
        var list = container.find('ul');

        if (list && list.length) {
            list.empty();
            container.addClass('validation-summary-valid').removeClass('validation-summary-errors');
        }
    }

});


$(document).on('keyup', '.registerfield input', function () {
    $.validator.unobtrusive.parse($('#registerForm'));
    console.log('register' + $('#registerForm').valid());
    let empty = false;
    $('.registerfield input').each(function () {
        empty = $(this).val().length == 0;
    });
    if (empty || !$('#registerForm').valid())
        $('.registeractions input').attr('disabled', 'disabled');
    else {
        $('.registeractions input').attr('disabled', false);
        var container = $('#registerForm').find('[data-valmsg-summary="true"]');
        var list = container.find('ul');

        if (list && list.length) {
            list.empty();
            container.addClass('validation-summary-valid').removeClass('validation-summary-errors');
        }
    }
});

loginFunc = (form) => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                location.reload();
            },
            error: function (xhr) {
                console.log(xhr)
                $('#loginModal').html(xhr.responseText);
            }
        })
    }
    catch (e) {
        console.log(e);
    }

    //to prevent default form submit event
    return false;

}

registerFunc = (form) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                location.reload();
            },
            error: function (xhr) {
                console.log(xhr)
                $('#loginModal').html(xhr.responseText);
            }
        })
    }
    catch (e) {
        console.log(e);
    }

    //to prevent default form submit event
    return false;
}