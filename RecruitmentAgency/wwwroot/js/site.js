// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function openVacancyDetails(id, showViewApplications) {
    $.ajax(`/Vacancies/Details/${id}${showViewApplications !== undefined ? `?showViewApplications=${showViewApplications}` : ''}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#vacancyDetails").modal();
            $('#vacancyDetails').on('hidden.bs.modal', function (e) {
                onModalClose('#vacancyDetails');
            })
        });
}

function openCreatePayment(vacancyId) {
    $.ajax(`/Payments/Create?vacancyId=${vacancyId}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#createPayment").modal();
            $('#createPayment').on('hidden.bs.modal', function (e) {
                onModalClose('#createPayment');
            })
        });
}

function openCustomerDetails(id, showIndebtedness) {
    $.ajax(`/Customers/Details/${id}${showIndebtedness !== undefined ? `?showIndebtedness=${showIndebtedness}` : ''}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#customerDetails").modal();
            $('#customerDetails').on('hidden.bs.modal', function (e) {
                onModalClose('#customerDetails');
            })
        });
}

function openCandidateDetails(id) {
    $.ajax(`/Candidates/Details/${id}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#candidateDetails").modal();
            $('#candidateDetails').on('hidden.bs.modal', function (e) {
                onModalClose('#candidateDetails');
            })
        });
}

function openRecruiterDetails(id) {
    $.ajax(`/Recruiters/Details/${id}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#recruiterDetails").modal();
            $('#recruiterDetails').on('hidden.bs.modal', function (e) {
                onModalClose('#recruiterDetails');
            })
        });
}

function openEmployeeDetails(id) {
    $.ajax(`/Employees/Details/${id}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#employeeDetails").modal();
            $('#employeeDetails').on('hidden.bs.modal', function (e) {
                onModalClose('#employeeDetails');
            })
        });
}

function openApplicationCreate(vacancyId) {
    $.ajax(`/Applications/Create?vacancyId=${vacancyId}`)
        .then((data) => {
            $('#modalSpace').append($(data));
            $("#createApplications").modal();
            $('#createApplications').on('hidden.bs.modal', function (e) {
                onModalClose('#createApplications');
            })
        });
}

function setApplicationStatus(appId, appStatusId) {
    $.ajax(`/Applications/${appId}/Status/${appStatusId}`, { method: 'POST' })
        .then((data) => {
                location.reload();
            });
}

function onModalClose(selector) {
    $(selector).remove();
}

function onSubmit(e) {
    e.stopPropagation();
    e.preventDefault();
    const action = $(e.target).attr('action');

    $.ajax(action, { method: 'POST', data: $(e.target).serialize() }).then(() => location.reload());
}

$(document).ready(function () {
    bsCustomFileInput.init();
})