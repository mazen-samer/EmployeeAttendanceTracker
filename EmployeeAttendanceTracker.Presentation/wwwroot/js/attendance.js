$(function () {
    // Initialize the datepicker calendar [cite: 39]
    $("#attendance-date").datepicker({
        dateFormat: "yy-mm-dd",
        maxDate: 0, // Disable future dates [cite: 41, 64]
        onSelect: function () {
            getAttendanceStatus(); // Fetch status when a new date is selected
        }
    });

    // Event handler for when the employee dropdown changes
    $("#employee-select").on("change", function () {
        getAttendanceStatus();
    });

    function getAttendanceStatus() {
        const employeeId = $("#employee-select").val();
        const date = $("#attendance-date").val();

        // Only proceed if both an employee and a date are selected
        if (!employeeId || !date) {
            $("#attendance-status-panel").hide();
            return;
        }

        $("#attendance-status-panel").show();

        // AJAX call to the controller to get the status
        $.ajax({
            url: '/Attendances/GetAttendanceStatus',
            type: 'GET',
            data: { employeeId: employeeId, date: date },
            success: function (response) {
                [cite_start]$("#status-text").text(response.status); // Update the status text on the page [cite: 42]
            }
        });
    }

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

        // AJAX call to record the attendance
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
                    // Refresh the status display after successfully recording
                    getAttendanceStatus();
                } else {
                    alert("Error: " + response.message);
                }
            }
        });
    }
});