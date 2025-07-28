$(function () {
    // This function will be the main trigger for all our logic
    function getAttendanceStatus() {
        const employeeId = $("#employee-select").val();
        const date = $("#attendance-date").val();

        // If either field is empty, hide the status panel and do nothing else.
        if (!employeeId || !date) {
            $("#attendance-status-panel").hide();
            return;
        }

        // If we have both values, show the panel and make the AJAX call.
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

    // Initialize the datepicker calendar
    $("#attendance-date").datepicker({
        dateFormat: "yy-mm-dd",
        maxDate: 0, // Disable future dates
        onSelect: getAttendanceStatus // Call our main function when a date is selected
    });

    // Call our main function when the employee dropdown changes
    $("#employee-select").on("change", getAttendanceStatus);


    // --- The rest of the file remains the same ---

    // Event handler for the "Mark as Present" button
    $("#mark-present").on("click", function () {
        recordAttendance(true);
    });

    // Event handler for the "Mark as Absent" button
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
});