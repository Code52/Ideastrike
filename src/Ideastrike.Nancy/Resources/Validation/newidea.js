var newIdeaFormValidation = (function () {
    $('form').submit(function () {
        var invalid = false;

        invalid = $('#title').val() == '';
        invalid = invalid || $('#new-idea').val() == '';

        if (invalid) {
            $('.error').show();
        }

        return !invalid;
    });

    $('#title, #new-idea').live('keyup', function () {
        if ($('#title').val().length > 0 && $('#new-idea').val().length > 0) {
            $('.error').hide();
        }
    });
})();