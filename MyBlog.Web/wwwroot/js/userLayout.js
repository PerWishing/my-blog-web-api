$(document).on('click', '#userProfileBtn', function () {
    $('.modal').modal('hide');
    username = $(this).attr('data-author');
    var pageUrl = window.location.href
    var urlArr = pageUrl.split('/')
    if (urlArr.indexOf("Us") > -1) {
        history.pushState(null, null, username);
    }
    else {
        history.pushState(null, null, "Us/Users/" + username);
    }
    $('#mainInLayout').load("/User/UserPublicProfile?username=" + username)
    console.log(username)
});

loadUserLayout = (username) => {
    console.log(username)

    $('#userInfo').load("/User/UserInfoPartial?username=" + username)

    $('#userPosts').load("/Post/UserPosts?username=" + username)

    $('#userReaders').load("/User/UserReaders?username=" + username)

    $('#userFollowed').load("/User/UserFollowed?username=" + username)

    $('#editUserModal').load("/User/EditUserProfile?username=" + username);

    $('#createPostModal').load("/Post/Create");

    $('#uploadUserAvatarModal').load("/Image/UploadAvatar?username=" + username);

    //UserLayout buttons
    $("#savedPostsButton").click(function () {
        $("#userPosts").load("/Post/UserSavedPosts?username=" + username)
    });

    $("#myPostsButton").click(function () {
        $("#userPosts").load("/Post/UserPosts?username=" + username)
    });

    //Img Load for Create Post

};

editUserFunc = form => {
    var username = $('#userEditHidden').val()
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $('#userInfo').load("/User/UserInfoPartial?username=" + username)
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

startOrStopReadUserFunc = form => {
    var username = $('#userPublicHidden').val()

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $('#userInfo').load("/User/UserInfoPartial?username=" + username);
                $.ajax({
                    type: "GET",
                    url: "/User/UserReaders?username=" + username,
                    success: function (getres) {
                        if (getres !== undefined) {
                            $("#userReaders").html(getres);
                        }
                        else {
                            $("#userReaders").html('');
                        }
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