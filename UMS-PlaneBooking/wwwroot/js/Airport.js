$(document).ready(function () {
    // Trigger the modal when the 'Add New Airport' button is clicked
    $("#openAddAirportModal").click(function (e) {
        e.preventDefault();  // Prevent default link behavior
        $("#addAirportModal").modal('show');  // Show the modal
    });
    // Reset the form fields when the modal is hidden (closed)
    $('#addAirportModal').on('hidden.bs.modal', function () {
        $('#addAirportForm')[0].reset(); // Reset the form when the modal is closed
    });
    // Close modal when "X" button or "Cancel" button is clicked
    $('.close, .btn-secondary').click(function () {
        $('#addAirportModal').modal('hide');
    });
    // Handle form submission
    $("#addAirportForm").submit(function (event) {
        debugger;
        event.preventDefault(); // Prevent the form from submitting the traditional way

        var airportName = $("#AirportName").val();
        var address = $("#Address").val();

        // Check if the airport name already exists
        $.ajax({
            type: "GET",
            url: "/Airport/GetByAirportName", // Create an endpoint for this check in your controller
            data: { airportName: airportName },
            success: function (response) {
                if (response.exists) {
                    alert("Airport name already exists. Please choose a different name.");
                } else {
                    // Proceed with form submission if the airport name is unique
                    var airportData = {
                        AirportName: airportName,
                        Address: address
                    };

                    $.ajax({
                        type: "POST",
                        url: "/Airport/AddAirport",
                        data: JSON.stringify(airportData),
                        contentType: "application/json",
                        success: function (response) {
                            if (response && response.message) {
                                alert(response.message);
                                $("#addAirportModal").modal('hide');
                                location.reload();
                            } else {
                                alert("Error: Something went wrong.");
                            }
                        },
                        error: function (xhr, status, error) {
                            console.log("AJAX error:", error);
                            alert("Error adding airport");
                        }
                    });
                }
            },
            error: function (xhr, status, error) {
                console.log("AJAX error:", error);
                alert("Error checking airport name");
            }
        });
    });

    // Edit event handler.
    $("body").on("click", ".edit", function () {
        var row = $(this).closest("tr");
        $("td", row).each(function () {
            if ($(this).find("input").length > 0) {
                $(this).find("input").show();
                $(this).find("span").hide();
            }
        });
        row.find(".update").show();
        row.find(".cancel").show();
        row.find(".delete").hide();
        $(this).hide();
    });

    // Update event handler.
    $("body").on("click", ".update", function () {
        var row = $(this).closest("tr");
        var airportData = {};
        $("td", row).each(function () {
            if ($(this).find("input").length > 0) {
                var span = $(this).find("span");
                var input = $(this).find("input");
                span.html(input.val());
                span.show();
                input.hide();
            }
        });
        row.find(".edit").show();
        row.find(".delete").show();
        row.find(".cancel").hide();
        $(this).hide();

        // Prepare airport data for updating
        airportData.AirportID = row.find(".airportId span").text();
        airportData.AirportName = row.find(".airportName input").val();  // Use input value here
        airportData.Address = row.find(".airportAddress input").val();  // Use input value here

        $.ajax({
            type: "POST",
            url: "/Airport/UpdateAirport", // Ensure this URL is correct
            data: JSON.stringify(airportData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // Handle successful update if needed
                alert("Airport updated successfully!");
            },
            error: function (xhr, status, error) {
                alert("An error occurred: " + error);
            }
        });
    });

    // Cancel event handler.
    $("body").on("click", ".cancel", function () {
        var row = $(this).closest("tr");
        $("td", row).each(function () {
            if ($(this).find("input").length > 0) {
                var span = $(this).find("span");
                var input = $(this).find("input");
                input.val(span.html());
                span.show();
                input.hide();
            }
        });
        row.find(".edit").show();
        row.find(".delete").show();
        row.find(".update").hide();
        $(this).hide();
    });

    // Delete event handler.
    $("body").on("click", "#tblAirport .delete", function () {
        debugger;
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var airportData = {};
            airportData.AirportID = row.find(".airportId span").text();
            $.ajax({
                type: "POST",
                url: "/Airport/DeleteAirport",
                data: JSON.stringify(airportData),   // Use JSON.stringify to pass the data correctly
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Check if the deletion was successful
                    if (response.message === "Airport deleted successfully.") {
                        // Remove the row from the table after successful deletion
                        row.remove();

                        // Optionally, show a success message to the user
                        alert(response.message);  // You can customize this or display it elsewhere
                    } else {
                        alert("Failed to delete the airport. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    // Handle any errors that might occur during the AJAX request
                    alert("An error occurred while deleting the row.");
                }
            });
        }
    });

});
