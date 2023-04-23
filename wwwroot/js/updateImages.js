$(document).ready(function () {
    $("#btnUpload").on('click', function () {

    });

    $("#fileUpload").on('change', function () {
        var files = $('#fileUpload').prop("files");
        var url = "/Product/UploadNewImages";
        formData = new FormData();

        for (f of files) {
            formData.append("images", f);
        }

        jQuery.ajax({
            type: 'POST',
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                $('#UploadImageResult').append(
                    '<div class="uploadwaiting col col-3 p-1"><img src="/img/upload.gif" class="mb-2 mx-auto" height="250"/></div>');
                $('input').attr("disabled", true);
            },
            success: function (repo) {
                $('#fileUpload').val('');
                $('.uploadwaiting').remove();
                $('input').attr("disabled", false);

                $('#UploadImageResult').append(repo);
            },
            error: function () {
                alert("Error occurs");
            }
        });

        $('#fileUpload').val('');

    });
    
    $(document).on('click','.closebutton', function () {
        DisableImage(this, true);
    });

    $(document).on('click', '.recoverbutton', function () {
        DisableImage(this, false);
    });

    function DisableImage(btn, disable) {
        var cont = $(btn).parents('.updatePhoto')[0];
        $(cont).children().attr("disabled", disable);

        var recover = $(cont).children('.recoverbutton');
        $(recover).attr("disabled", !disable);
    }

    $("#form").submit(function (eventObj) {

        var images = $('.updatePhoto');
        var allProdPhotos = [];
        var photoToDelete = [];

        images.each(function () {
            var select = $(this).children('select')[0];
            if ($(select).attr('disabled')) {
                photoToDelete.push(this.id);
            } else {
                allProdPhotos.push({
                    Name: this.id,
                    PhotoType: parseInt(select.value)
                })
            }
        });

        for (p of allProdPhotos) {
            $("<input />").attr("type", "hidden")
                .attr("name", "allPhotos")
                .attr("value", JSON.stringify(p))
                .appendTo("#form");
        }

        for (p of photoToDelete) {
            $("<input />").attr("type", "hidden")
                .attr("name", "photoToDelete")
                .attr("value", p)
                .appendTo("#form");
        }

        return true;
    });

});