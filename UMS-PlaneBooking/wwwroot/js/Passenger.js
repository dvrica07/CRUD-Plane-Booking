$(document).ready(function () {
    let isEditing = false;
    let currentBookingId = null;

    $("#openAddPassengerModal").click(function (e) {
        e.preventDefault(); 
        isEditing = false; 
        $("#addPassengerModal").modal('show');  
        $('#addPassengerForm')[0].reset(); 
    });


    $('#addPassengerModal').on('hidden.bs.modal', function () {
        $('#addPassengerForm')[0].reset(); 
    });

    $('.close, .btn-secondary').click(function () {
        $('#addPassengerModal').modal('hide');
    });

    // Handle form submission
    $("#addPassengerForm").submit(function (event) {
        event.preventDefault(); 

        var passengerName = $("#PassengerName").val();
        var selectedFlightID = $("#inputGroupSelect01").val();

        $(".input-group").removeClass("is-invalid");
        $("select").removeClass("is-invalid");

        var isValid = true;

        if (!selectedFlightID || selectedFlightID === "Choose Flight") {
            $("#inputGroupSelect01").addClass("is-invalid"); 
            isValid = false;
        }

        if (!isValid) {
            return; 
        }

        var passengerData = {
            PassengerName: passengerName,
            FlightID: selectedFlightID,
        };

        if (isEditing) {
            passengerData.BookingID = currentBookingId;
        }

        $.ajax({
            type: "POST",
            url: isEditing ? "/Passenger/UpdatePassenger" : "/Passenger/AddPassenger", 
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

    // Update passenger function
    $(".edit").click(function () {
        isEditing = true;
        var row = $(this).closest("tr");

        currentBookingId = row.find(".bookingId span").text();
        var passengerName = row.find(".passengerName span").text();
        var flightId = row.find(".flightCode input").attr("id");

        $("#PassengerName").val(passengerName);
        $("#inputGroupSelect01").val(flightId); 

        // Show the modal for editing
        $("#addPassengerModal").modal('show');
    });

    // Delete event handler.
    $("body").on("click", "#tblPassenger .delete", function () {
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var passengerData = {};
            passengerData.bookingID = row.find(".bookingId span").text();
            $.ajax({
                type: "POST",
                url: "/Passenger/DeletePassenger",
                data: JSON.stringify(passengerData),  
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.message === "Passenger deleted successfully.") {
                        row.remove();

                        alert(response.message);
                    } else {
                        alert("Failed to delete the passenger. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while deleting the row.");
                }
            });
        }
    });
});