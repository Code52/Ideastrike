var newIdeaFormValidation = (function () {
    $('form').submit(function () {
        var invalid = false;

        if ($('#title').val() == '') {
            invalid = true;
            $('#title').closest('.clearfix').addClass('error');
        }

        if ($('#new-idea').val() == '') {
            invalid = true;
            $('#new-idea').closest('.clearfix').addClass('error');
        }

        if (invalid) {
            $('.alert-message').show();
        }

        return !invalid;
    });

    $('#title, #new-idea').live('keyup', function () {
        if ($(this).val().length > 0) {
            $(this).closest('.clearfix').removeClass('error');
            $('.alert-message').hide();
        }
    });

})();