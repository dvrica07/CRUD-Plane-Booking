$(document).ready(function () {
    let isEditing = false;
    let currentBookingId = null;

    // Trigger the modal when the 'Add New Airport' button is clicked
    $("#openAddPassengerModal").click(function (e) {
        e.preventDefault();  // Prevent default link behavior
        isEditing = false; // Reset the flag to "add"
        $("#addPassengerModal").modal('show');  // Show the modal
        $('#addPassengerForm')[0].reset(); // Reset the form when adding a new flight
    });

    // Reset the form fields when the modal is hidden (closed)
    $('#addPassengerModal').on('hidden.bs.modal', function () {
        $('#addPassengerForm')[0].reset(); // Reset the form when the modal is closed
    });

    // Close modal when "X" button or "Cancel" button is clicked
    $('.close, .btn-secondary').click(function () {
        $('#addPassengerModal').modal('hide');
    });

    // Handle form submission
    // Add new Flight function
    $("#addPassengerForm").submit(function (event) {
        debugger;
        event.preventDefault(); // Prevent the form from submitting the traditional way

        var passengerName = $("#PassengerName").val();
        var selectedFlightID = $("#inputGroupSelect01").val();

        // Reset any previous error states
        $(".input-group").removeClass("is-invalid");
        $("select").removeClass("is-invalid");

        var isValid = true;

        // Validation for the flight dropdown
        if (!selectedFlightID || selectedFlightID === "Choose Flight") {
            $("#inputGroupSelect01").addClass("is-invalid"); // Add class directly to the select
            isValid = false;
        }

        if (!isValid) {
            return; // Stop the form submission if validation fails
        }

        var passengerData = {
            PassengerName: passengerName,
            FlightID: selectedFlightID,
        };

        // If editing, include the bookingId in the data
        if (isEditing) {
            passengerData.BookingID = currentBookingId;
        }

        $.ajax({
            type: "POST",
            url: isEditing ? "/Passenger/UpdatePassenger" : "/Passenger/AddPassenger", // Different endpoint for updating
            data: JSON.stringify(passengerData),
            contentType: "application/json",
            success: function (response) {
                if (response && response.message) {
                    alert(response.message);
                    $("#addPassengerModal").modal('hide');
                    location.reload();
                } else {
                    alert("Error: Something went wrong.");
                }
            },
            error: function (xhr, status, error) {
                console.log("AJAX error:", error);
                alert("Error adding or updating passenger");
            }
        });
    });

    // Function to open the modal and populate it for editing a passenger
    // Update passenger function
    $(".edit").click(function () {
        debugger;
        isEditing = true; // Set the flag to "edit"
        var row = $(this).closest("tr");

        // Ensure the passenger data exists and is being populated correctly
        currentBookingId = row.find(".bookingId span").text();
        var passengerName = row.find(".passengerName span").text();
        var flightId = row.find(".flightCode input").attr("id");

        // Populate the modal fields with the retrieved values
        $("#PassengerName").val(passengerName);
        $("#inputGroupSelect01").val(flightId); 

        // Show the modal for editing
        $("#addPassengerModal").modal('show');
    });

    // Delete event handler.
    // Delete passenger function
    $("body").on("click", "#tblPassenger .delete", function () {
        debugger;
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var passengerData = {};
            passengerData.bookingID = row.find(".bookingId span").text();
            $.ajax({
                type: "POST",
                url: "/Passenger/DeletePassenger",
                data: JSON.stringify(passengerData),   // Use JSON.stringify to pass the data correctly
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Check if the deletion was successful
                    if (response.message === "Passenger deleted successfully.") {
                        // Remove the row from the table after successful deletion
                        row.remove();

                        // Optionally, show a success message to the user
                        alert(response.message);  // You can customize this or display it elsewhere
                    } else {
                        alert("Failed to delete the passenger. Please try again.");
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