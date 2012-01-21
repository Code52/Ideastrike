
function toggleExpander() {
    var elem = $(this);
    var other = $(elem.data("element"));

    if (other.is(":visible")) {
        other.hide();
    } else {
        other.show();
    }
}

function previewText() {
    var elem = $(this);
    var other = $(elem.data("other"));
    var editor = $(elem.data("editor"));
    var view = $(elem.data("view"));

    var converter = new Showdown.converter();
    var formatted = converter.makeHtml(editor.val());
    view.html(formatted);
    editor.hide();
    view.show();
    elem.attr("disabled", "disabled");
    other.removeAttr("disabled");
}

function editText() {
    var elem = $(this);
    var other = $(elem.data("other"));
    var editor = $(elem.data("editor"));
    var view = $(elem.data("view"));
    // clear markdown
    view.html('');
    editor.show();
    view.hide();
    elem.attr("disabled", "disabled");
    other.removeAttr("disabled");
}