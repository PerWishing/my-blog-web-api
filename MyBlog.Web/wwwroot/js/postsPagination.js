$(document).on('click', '#nextPageBtn', function () {
    var nextPage = $(this).attr('data-next-page');
    $('#mainInLayout').load("Post/AllPosts?page=" + nextPage + "&search=" + $("#mainSearch").val().replace(/ /g, '%20'))
});

$(document).on('click', '#prevPageBtn', function () {
    var prevPage = $(this).attr('data-next-page');
    $('#mainInLayout').load("Post/AllPosts?page=" + prevPage + "&search=" + $("#mainSearch").val().replace(/ /g, '%20'))
});

$(document).on('click', '#nextPageForUserPostsBtn', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
    }

    var nextPage = $(this).attr('data-next-page');
    if ($('#IsSavedPostsHidden').val() < 1) {
        $('#userPosts').load("/Post/UserPosts?username=" + username + "&page=" + nextPage)
    }
    else {
        $('#userPosts').load("/Post/UserSavedPosts?username=" + username + "&page=" + nextPage)
    }
});

$(document).on('click', '#prevPageForUserPostsBtn', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
    }
    var prevPage = $(this).attr('data-next-page');
    if ($('#IsSavedPostsHidden').val() < 1) {
        $('#userPosts').load("/Post/UserPosts?username=" + username + "&page=" + prevPage)
    }
    else {
        $('#userPosts').load("/Post/UserSavedPosts?username=" + username + "&page=" + prevPage)
    }
});