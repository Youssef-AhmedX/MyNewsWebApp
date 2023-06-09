var rowUpdated;
var newRow;
var datatable;

function showSuccessMessage(message = 'Saved Successfully!') {


    Swal.fire({
        title: "Success",
        text: message,
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-primary"
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('.js-edition-status').parents('tr').removeClass('animate__animated animate__flash');
            $('tbody').find('.js-new-row').removeClass('animate__animated animate__flash');
            $('tbody').find('.js-new-row').removeClass('js-new-row');
        }

    });
}


function showErorrMessage(message) {

    if (message.responseText == '') {
        message.responseText = 'Something Went Wrong!';
    }

    Swal.fire({
        title: "Ooops...!",
        text: message.responseText,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}
function disableSubmitBtn() {
    $('body :submit').attr('disabled', 'disabled');
    $('.indicator-progress').removeClass('d-none');
    $('.indicator-label').addClass('d-none');

}

function onModalBegin() {

    disableSubmitBtn();
}

function modalSubmitSuccess(row) {
    $('#Modal').modal('hide');
    addNewRow(row);
    $('tbody').find('.js-new-row').addClass('animate__animated animate__flash');
    showSuccessMessage();

}


function onModalComplete() {

    $('body :submit').removeAttr('disabled', 'disabled');
    $('.indicator-label').removeClass('d-none');
    $('.indicator-progress').addClass('d-none');

}

function select2func() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {

        $('form').not('.js-logOutForm').validate().element('#' + $(this).attr('id'));

    });
}

function deleteRow() {

    datatable.row(rowUpdated).remove().draw()
    rowUpdated = undefined;
}

function addNewRow(row) {

    var newRow = $(row);

    datatable.row.add(newRow).draw();

    if (rowUpdated !== undefined) {
        datatable.row(rowUpdated).remove().draw()

        rowUpdated = undefined;
    }
}

$(document).ready(function () {

    //SignOut

    $('.js-logOut').on('click', function () {

        $('.js-logOutForm').submit();

    });


    ////select2

    select2func();

    //Disable Submit Button

    $('form').not('.js-logOutForm').on('submit', function () {

        var isValid = $(this).valid();

        if (isValid) disableSubmitBtn();

    });

    //DateRangePicker

    $('.js-daterangepicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoApply: true,
        drops: 'auto',
    });


    $('body').delegate('.js-change-btn', 'click', function () {

        var btn = $(this);

        bootbox.confirm({
            title: btn.data('title'),
            message: btn.data('message'),
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel',
                    className: 'btn-primary'

                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {

                    $.post
                        (
                            {
                                url: btn.data('url'),
                                data: {
                                    '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                                },
                                success: function (row) {
                                    if (btn.data('is-deleted') === 'redirect') {
                                        window.location.replace("/News/Index");
                                    }
                                    else {
                                        rowUpdated = btn.parents('tr');
                                        deleteRow()
                                        showSuccessMessage();
                                    }


                                },
                                error: function (message) {
                                    showErorrMessage(message);
                                }

                            }

                        )

                }
            }
        });

    });

    //Handle Modal
    $('body').delegate('.js-render-modal', 'click', function () {

        var btn = $(this);
        var modal = $('#Modal');

        if (btn.data('is-edited') !== undefined) {
            rowUpdated = btn.parents('tr');
            rowUpdated.removeClass('animate__animated animate__flash');

        }


        $.get({

            url: btn.data('url'),

            success: function (form) {
                modal.find('.modal-title').text(btn.data('title'));
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
                modal.modal('show');


            },

            error: function (message) {
                showErorrMessage(message);
            }
        })



    });

});
