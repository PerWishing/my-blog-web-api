$('#mainSearch').on('click', function (e) {
    $('#usersSearch').val('')
    $('#mainInLayout').load("Post/AllPosts?page=" + 1)
});

$('#mainSearch').on('keyup', function (e) {
    if ($(this).val().length > 3) {
        $('#mainInLayout').load("Post/AllPosts?page=1&search=" + $(this).val().replace(/ /g, '%20'))
    }
    else if ($(this).val().length < 1) {
        $('#mainInLayout').load("Post/AllPosts?page=" + 1)
    }
});

$(document).on('click', '.js-nav-btn', function () {
    $('#mainSearch').val('')
    $('#usersSearch').val('')
});