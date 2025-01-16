$(document).ready(function () {
    let isEditing = false; 
    let currentFlightId = null; 

      // Trigger the modal when the 'Add New Airport' button is clicked
    $("#openAddFlightModal").click(function (e) {
        e.preventDefault();  // Prevent default link behavior
        isEditing = false; // Reset the flag to "add"
        $("#addFlightModal").modal('show');  // Show the modal
        $('#addFlightForm')[0].reset(); // Reset the form when adding a new flight
    });

    // Reset the form fields when the modal is hidden (closed)
    $('#addFlightModal').on('hidden.bs.modal', function () {
        $('#addFlightForm')[0].reset(); // Reset the form when the modal is closed
    });

    // Close modal when "X" button or "Cancel" button is clicked
    $('.close, .btn-secondary').click(function () {
        $('#addFlightModal').modal('hide');
    });

    // Handle form submission
    // Add new Flight function
    $("#addFlightForm").submit(function (event) {
        debugger;
        event.preventDefault(); // Prevent the form from submitting the traditional way

        var flightCode = $("#FlightCode").val();
        var selectedAirportID = $("#inputGroupSelect01").val();
        var selectedPlaneID = $("#inputGroupSelect02").val();

        // Reset any previous error states
        $(".input-group").removeClass("is-invalid");
        $("select").removeClass("is-invalid");

        var isValid = true;

        // Validation for the airport dropdown
        if (!selectedAirportID || selectedAirportID === "Choose Airport") {
            $("#inputGroupSelect01").addClass("is-invalid"); // Add class directly to the select
            isValid = false;
        }

        // Validation for the plane dropdown
        if (!selectedPlaneID || selectedPlaneID === "Choose Plane") {
            $("#inputGroupSelect02").addClass("is-invalid"); // Add class directly to the select
            isValid = false;
        }

        if (!isValid) {
            return; // Stop the form submission if validation fails
        }

        var flightData = {
            FlightCode: flightCode,
            AirportID: selectedAirportID,
            PlaneID: selectedPlaneID,
        };

        // If editing, include the flightId in the data
        if (isEditing) {
            flightData.FlightID = currentFlightId;
        }

        $.ajax({
            type: "POST",
            url: isEditing ? "/Flight/UpdateFlight" : "/Flight/AddFlight", // Different endpoint for updating
            data: JSON.stringify(flightData),
            contentType: "application/json",
            success: function (response) {
                if (response && response.message) {
                    alert(response.message);
                    $("#addFlightModal").modal('hide');
                    location.reload();
                } else {
                    alert("Error: Something went wrong.");
                }
            },
            error: function (xhr, status, error) {
                console.log("AJAX error:", error);
                alert("Error adding or updating flight");
            }
        });
    });

    // Function to open the modal and populate it for editing a flight
    // Update flight function
    $(".edit").click(function () {
        debugger;
        isEditing = true; // Set the flag to "edit"
        var row = $(this).closest("tr");

        // Ensure the flight data exists and is being populated correctly
        currentFlightId = row.find(".flightId span").text();
        var flightCode = row.find(".flightCode span").text();
        var airportID = row.find(".airportName input").attr("id");
        var planeID = row.find(".planeAirline input").attr("id");

        // Populate the modal fields with the retrieved values
        $("#FlightCode").val(flightCode);
        $("#inputGroupSelect01").val(airportID); // Make sure this matches the value in the dropdown options
        $("#inputGroupSelect02").val(planeID);   // Make sure this matches the value in the dropdown options

        // Show the modal for editing
        $("#addFlightModal").modal('show');
    });

    // Delete event handler.
    // Delete flight function
    $("body").on("click", "#tblFlight .delete", function () {
        debugger;
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var flightData = {};
            flightData.flightID = row.find(".flightId span").text();
            $.ajax({
                type: "POST",
                url: "/Flight/DeleteFlight",
                data: JSON.stringify(flightData),   // Use JSON.stringify to pass the data correctly
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Check if the deletion was successful
                    if (response.message === "Flight deleted successfully.") {
                        // Remove the row from the table after successful deletion
                        row.remove();

                        // Optionally, show a success message to the user
                        alert(response.message);  // You can customize this or display it elsewhere
                    } else {
                        alert("Failed to delete the flight. Please try again.");
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