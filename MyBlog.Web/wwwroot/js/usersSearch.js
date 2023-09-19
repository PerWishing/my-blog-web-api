$(document).on('click', '#usersSearch', function () {
    $('#mainSearch').val('')
    $('#mainInLayout').load("User/Users")
});

$(document).on('keyup', '#usersSearch', function () {
    if ($(this).val().length > 3) {
        $('#mainInLayout').load("User/Users?search=" + $(this).val().replace(/ /g, '%20'))
    }
    else if ($(this).val().length < 2) {
        console.log($(this).val());
        $('#mainInLayout').load("User/Users")
    }
});

$(document).on('input', '#readersSearch', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
    }

    if ($(this).val().length > 3) {
        $('#userReaders').load("User/UserReaders?username=" + username + "&search=" + $(this).val().replace(/ /g, '%20'))
    }
    else if ($(this).val().length < 2) {
        console.log($(this).val());
        $('#userReaders').load("User/UserReaders?username=" + username)
    }
});

$(document).on('input', '#followedSearch', function () {
    var username = ''
    if ($('#userPublicHidden').length > 0) {
        username = $('#userPublicHidden').val()
    }
    else {
        username = $('#userMainHidden').val()
    }
    if ($(this).val().length > 3) {
        $('#userFollowed').load("User/UserFollowed?username=" + username + "&search=" + $(this).val().replace(/ /g, '%20'))
    }
    else if ($(this).val().length < 2) {
        console.log($(this).val());
        $('#userFollowed').load("User/UserFollowed?username=" + username)
    }
});