$(document).on('click', '#likeBtn', function () {
    $(this).toggleClass('btn-secondary');
    $(this).toggleClass('btn-danger');
    commentid = $(this).attr('data-commentid');
    isLiked = parseInt($(this).attr('data-isliked'));
    likesCount = parseInt($(this).parent('div').find("#likesCount").text());
    if (isLiked > 0) {
        likeUrl = "/Comment/DeleteLike?id=" + commentid
        $(this).parent('div').find("#likesCount").text(likesCount - 1)
        $(this).attr('data-isliked','0')
    }
    if (isLiked < 1) {
        likeUrl = "/Comment/LikeComment?id=" + commentid
        $(this).parent('div').find("#likesCount").text(likesCount + 1)
        $(this).attr('data-isliked', '1')
    }
    console.log($(this).parent('div').find("#likesCount").text());
    try {
        $.ajax({
            type: "POST",
            url: likeUrl,
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {

            },
            error: function (err) {
                console.log(err);
            }
        })
    }
    catch (e) {
        console.log(e);
    }
});

getCreateCommentView = (url) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#createComment").html(res);
        }
    })
};

createCommentFunc = (form, postId, postauthor) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $('#postComments').load("/Comment/PostComments?postid=" + postId + "&postauthor=" + postauthor)
                $('#createComment').load("/Comment/Create?postid=" + postId + "&postauthor=" + postauthor)
                $('#createComment').hide()
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

deleteCommentFunc = (form, postId, postauthor) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $.ajax({
                    type: "GET",
                    url: "/Comment/PostComments?postid=" + postId + "&postauthor=" + postauthor,
                    success: function (getres) {
                        if (getres !== undefined) {
                            $("#postComments").html(getres);
                        }
                        else {
                            $("#postComments").html('');
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                })
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