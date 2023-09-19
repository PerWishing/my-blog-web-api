$(document).on('click', '.js-card', function () {
    postid = $(this).attr('data-postid');
    postauthor = $(this).attr('data-postauthor');
    console.log('openPost')
    $('#openPost').load("/Post/Index?id=" + postid, function () {
        $('#userHeadPartial').load("/User/UserHead?username=" + postauthor);

        $('#savePartial').load("/Post/SavePartial?id=" + postid + "&postauthor=" + postauthor);

        $('#postComments').load("/Comment/PostComments?postid=" + postid + "&postauthor=" + postauthor)
        $('#createComment').load("/Comment/Create?postid=" + postid + "&postauthor=" + postauthor)
        $('#createComment').hide()
    });
});

$(document).on('click', '#hideshow', function () {
    $('#createComment').toggle('hide');
});

$(document).on('click', '#deletePostBtn', function () {
    postid = $("#Id").val();
    try {
        $.ajax({
            type: "POST",
            url: '/Post/Delete/' + postid,
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {
                //$('#cardDiv-' + postid).remove();
                $("#myMasonry").masonry('remove', $('#cardDiv-' + postid)).masonry();
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
});

saveOrDeletePostFunc = (form, postid) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $('#savePartial').load("/Post/SavePartial?id=" + postid);
                try {
                    if ($("#mainView").length > 0) {
                        var username = $('#userMainHidden').val()
                        $("#mainView").load("/Post/UserSavedPosts?username=" + username)
                        $('.modal').modal('hide');
                    }
                }
                catch (e) {
                    console.log(e);
                }
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