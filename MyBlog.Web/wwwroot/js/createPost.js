$(document).on('keyup', '.cpostfield input, .cpostfield textarea', function () {
    $.validator.unobtrusive.parse($('#formCreatePost'));
    let empty = false;
    $('.cpostfield input').each(function () {
        empty = $(this).val().length == 0;
    });
    if (empty || !$('#formCreatePost').valid())
        $('.cpostactions input').attr('disabled', 'disabled');
    else {
        $('.cpostactions input').attr('disabled', false);
    }
});

$(document).on("hidden.bs.modal", "#createPostModal", function () {
    $('#createPostModal').html('');
    $('#file').remove();
    $('#createPostModal').load("/Post/Create");
});

$(document).on('click', '#subCreatePost', function (evt) {
    evt.preventDefault();
    var form = $("#formCreatePost")[0];
    var data = new FormData(form);


    //Debuging FormData
    for (var p of data) {
        console.log(p);
    }

    $.ajax({
        type: "POST",
        url: '/Post/Create',
        data: data,
        processData: false,
        contentType: false,
        success: function (result) {
            $('#userPosts').load("/Post/UserPosts?username=" + $("#userMainHidden").val())
        }
    });

});

var abc = 0;
$(document).on('click', '#add_more', function () {
    $(this).before($("<div/>", { id: 'filediv' }).fadeIn('slow').append($("<input/>",
        {
            name: 'images',
            type: 'file',
            id: 'file',
            class: 'js-file'
        }).click().hide(),
        $("<br/><br/>")
    ));
});
$(document).on('change', '.js-file', function () {
    if (this.files && this.files[0]) {
        abc += 1; //increementing global variable by 1
        var z = abc - 1;
        var x = $(this)
            .parent()
            .find('#previewimg' + z).remove();
        $(this).before("<div id='abcd" + abc + "' class='abcd'><img id='previewimg" + abc + "' src='' class='img-fluid'/></div>");
        var reader = new FileReader();
        reader.onload = imageIsLoaded;
        reader.readAsDataURL(this.files[0]);
        $(this)
            .hide();
        $("#abcd" + abc).append($('<button class="btn btn-danger">Remove</button>',
        ).click(function () {
            $(this)
                .parent()
                .parent()
                .remove();
            if ($('input[type="file"]').length > 3) {
                $('#add_more').attr('disabled', 'disabled');
            }
            else {
                $('#add_more').attr('disabled', false);
            }
        }));

        if ($('input[type="file"]').length > 3) {
            $('#add_more').attr('disabled', 'disabled');
        }
        else {
            $('#add_more').attr('disabled', false);
        }
    }
});
//image preview
function imageIsLoaded(e) {
    $('#previewimg' + abc)
        .attr('src', e.target.result);
};