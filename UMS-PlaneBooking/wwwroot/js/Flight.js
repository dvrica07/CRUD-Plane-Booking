$(document).ready(function () {
    let isEditing = false; 
    let currentFlightId = null; 

    $("#openAddFlightModal").click(function (e) {
        e.preventDefault(); 
        isEditing = false; 
        $("#addFlightModal").modal('show'); 
        $('#addFlightForm')[0].reset(); 
    });
    $('#addFlightModal').on('hidden.bs.modal', function () {
        $('#addFlightForm')[0].reset(); 
    });

    $('.close, .btn-secondary').click(function () {
        $('#addFlightModal').modal('hide');
    });

    // Handle form submission
    $("#addFlightForm").submit(function (event) {
        event.preventDefault();

        var flightCode = $("#FlightCode").val();
        var selectedAirportID = $("#inputGroupSelect01").val();
        var selectedPlaneID = $("#inputGroupSelect02").val();

        // Reset any previous error states
        $(".input-group").removeClass("is-invalid");
        $("select").removeClass("is-invalid");

        var isValid = true;

        // Validation for the airport dropdown
        if (!selectedAirportID || selectedAirportID === "Choose Airport") {
            $("#inputGroupSelect01").addClass("is-invalid");
            isValid = false;
        }

        // Validation for the plane dropdown
        if (!selectedPlaneID || selectedPlaneID === "Choose Plane") {
            $("#inputGroupSelect02").addClass("is-invalid"); 
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        var flightData = {
            FlightCode: flightCode,
            AirportID: selectedAirportID,
            PlaneID: selectedPlaneID,
        };

        if (isEditing) {
            flightData.FlightID = currentFlightId;
        }

        $.ajax({
            type: "POST",
            url: isEditing ? "/Flight/UpdateFlight" : "/Flight/AddFlight", 
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

    // Update flight function
    $(".edit").click(function () {
        isEditing = true; 
        var row = $(this).closest("tr");

        // Ensure the flight data exists and is being populated correctly
        currentFlightId = row.find(".flightId span").text();
        var flightCode = row.find(".flightCode span").text();
        var airportID = row.find(".airportName input").attr("id");
        var planeID = row.find(".planeAirline input").attr("id");

        // Populate the modal fields with the retrieved values
        $("#FlightCode").val(flightCode);
        $("#inputGroupSelect01").val(airportID); 
        $("#inputGroupSelect02").val(planeID);  

        // Show the modal for editing
        $("#addFlightModal").modal('show');
    });

    // Delete flight function
    $("body").on("click", "#tblFlight .delete", function () {
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var flightData = {};
            flightData.flightID = row.find(".flightId span").text();
            $.ajax({
                type: "POST",
                url: "/Flight/DeleteFlight",
                data: JSON.stringify(flightData),  
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.message === "Flight deleted successfully.") {
                        row.remove();
                        alert(response.message);
                    } else {
                        alert("Failed to delete the flight. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while deleting the row.");
                }
            });
        }
    });
});