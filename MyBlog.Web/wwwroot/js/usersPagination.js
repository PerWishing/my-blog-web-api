$(document).on('click', '.js-users-nextPageForUserBtn', function () {
    var nextPage = $(this).attr('data-next-page');
    $('#mainInLayout').load("/User/Users?page=" + nextPage + "&search=" + $("#usersSearch").val().replace(/ /g, '%20'))
});
$(document).on('click', '.js-users-prevPageForUserBtn', function () {
    var prevPage = $(this).attr('data-next-page');
    $('#mainInLayout').load("/User/Users?page=" + prevPage + "&search=" + $("#usersSearch").val().replace(/ /g, '%20'))
});

$(document).on('click', '.js-nextPageForUserBtn', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
    }
    var nextPage = $(this).attr('data-next-page');
    if ($(this).attr('data-reader-or-followed') < 1) {
        $('#userReaders').load("/User/UserReaders?username=" + username + "&page=" + nextPage + "&search=" + $("#readersSearch").val().replace(/ /g, '%20'))
    }
    else {
        $('#userFollowed').load("/User/UserFollowed?username=" + username + "&page=" + nextPage + "&search=" + $("#followedSearch").val().replace(/ /g, '%20'))
    }
});

$(document).on('click', '.js-prevPageForUserBtn', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
        search = $("#readersSearch").val().replace(/ /g, '%20')
    }

    var prevPage = $(this).attr('data-next-page');
    if ($(this).attr('data-reader-or-followed') < 1) {
        $('#userReaders').load("/User/UserReaders?username=" + username + "&page=" + prevPage + "&search=" + $("#readersSearch").val().replace(/ /g, '%20'))
    }
    else {
        $('#userFollowed').load("/User/UserFollowed?username=" + username + "&page=" + prevPage + "&search=" + $("#followedSearch").val().replace(/ /g, '%20'))
    }
});