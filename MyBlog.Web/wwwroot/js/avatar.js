function loadAvatarScript(){
    //Get Username from hidden input
    var username = $("#Username").val();
    //initialize Croppie
    var basic = $('#main-cropper').croppie
        ({
            viewport: { width: 300, height: 300 },
            boundary: { width: 500, height: 400 },
            showZoomer: true,
            url: '/avatars/avatarSample.png',
            format: 'png' //'jpeg'|'png'|'webp'
        });

    //Reading the contents of the specified Blob or File
    function readFile(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#main-cropper').croppie('bind', {
                    url: e.target.result
                });
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    // Change Event to Read file content from File input
    $('#select').on('change', function () { readFile(this); });

    // Upload button to Post Cropped Image to Store.
    $('#btnupload').on('click', function () {
        basic.croppie('result', 'blob').then(function (blob) {
            var formData = new FormData();

            formData.append('username', username);
            formData.append('filename', username + '.png');
            formData.append('blob', blob);

            try {
                $.ajax({
                    type: "POST",
                    url: '/Image/UploadAvatar/',
                    //url: '@Url.Action("UploadAvatar", "Image")',
                    data: formData,
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
        });
    });


};