$(document).on('keyup', '.epostfield input, .epostfield textarea', function () {
    $.validator.unobtrusive.parse($('#formEditPost'));
    let empty = false;
    $('.epostfield input').each(function () {
        empty = $(this).val().length == 0;
    });
    if (empty || !$('#formEditPost').valid())
        $('.epostactions input').attr('disabled', 'disabled');
    else {
        $('.epostactions input').attr('disabled', false);
    }
});

editInPopup = (url) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#openPost").html(res);
        }
    })
};

editPostFunc = (form, postid) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $('#openPost').load("/Post/Index?id=" + postid);
                $('#cardDiv-' + postid).load("/Post/PostCard?id=" + postid);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
    catch (e) {
        console.log(e);
    }

    //to prevent default form submit event
    return false;
}