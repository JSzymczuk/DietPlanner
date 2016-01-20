var G_USE_LOADING_MODAL = true;

$("#search-keyword").keyup(function () { submitForm(false); });
$("#search-category").change(function () { submitForm(); });
$("#search-page-size").change(function () { submitForm(); });
$("#search-submit").click(function () { submitForm(); });
$("#search-clear").click(function () { clearAdvanced(); });

function selectCategory(id) {
    var pickCategory = $("#search-category");
    if (pickCategory.val() != id) {
        pickCategory.val(id);
        submitForm();
    }
}
function submitForm(useLoadingModal) {
    if (typeof (useLoadingModal) === 'undefined') {
        useLoadingModal = true;
    }
    G_USE_LOADING_MODAL = useLoadingModal;
    $("#search-form").submit();
}
function clearAdvanced() {
    var adv = $("#search-advanced");
    var children = adv.children("input");
    for (i = 0; i < children.length; i++) {
        if (children[i].type == "number") {
            children[i].value = null;
        }
    }
    $("#glutenFree").attr("checked", false);
    $("#sortType").val("NameAscending");
}
function showAdvanced() {
    $("#show-advanced").hide();
    $("#search-advanced").show();
}
function hideAdvanced() {
    $("#search-advanced").hide();
    clearAdvanced();
    $("#show-advanced").show();
}
