$(function () {
    function getAttendanceStatus() {
        const employeeId = $("#employee-select").val();
        const date = $("#attendance-date").val();

        if (!employeeId || !date) {
            $("#attendance-status-panel").hide();
            return;
        }

        $("#attendance-status-panel").show();

        $.ajax({
            url: '/Attendances/GetAttendanceStatus',
            type: 'GET',
            data: { employeeId: employeeId, date: date },
            success: function (response) {
                $("#status-text").text(response.status).removeClass('text-danger');
                if (response.status === 'Future Date') {
                    $("#status-text").addClass('text-danger');
                    $("#action-buttons").hide();
                } else {
                    $("#action-buttons").show();
                }
            },
            error: function () {
                alert("An error occurred while fetching the attendance status.");
            }
        });
    }

    $("#attendance-date").datepicker({
        dateFormat: "yy-mm-dd",
        maxDate: 0,
        onSelect: getAttendanceStatus
    });

    $("#employee-select").on("change", getAttendanceStatus);


    $("#mark-present").on("click", function () {
        recordAttendance(true);
    });

    $("#mark-absent").on("click", function () {
        recordAttendance(false);
    });

    function recordAttendance(isPresent) {
        const employeeId = $("#employee-select").val();
        const date = $("#attendance-date").val();

        if (!employeeId || !date) {
            alert("Please select an employee and a date.");
            return;
        }

        $.ajax({
            url: '/Attendances/RecordAttendance',
            type: 'POST',
            data: {
                employeeId: employeeId,
                date: date,
                isPresent: isPresent
            },
            success: function (response) {
                if (response.success) {
                    getAttendanceStatus();
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                alert("An error occurred while recording attendance.");
            }
        });
    }

    $("#delete-attendance").on("click", function () {
        const employeeId = $("#employee-select").val();
        const date = $("#attendance-date").val();

        if (!employeeId || !date) {
            alert("Please select an employee and a date.");
            return;
        }

        if (confirm("Are you sure you want to clear the attendance status for this date?")) {
            $.ajax({
                url: '/Attendances/DeleteAttendance',
                type: 'POST',
                data: {
                    employeeId: employeeId,
                    date: date
                },
                success: function (response) {
                    if (response.success) {
                        getAttendanceStatus();
                    } else {
                        alert("Error: " + response.message);
                    }
                },
                error: function () {
                    alert("An error occurred while deleting the attendance record.");
                }
            });
        }
    });
});