$(document).ready(function () { 
    $("#openAddAirportModal").click(function (e) {
        e.preventDefault();  
        $("#addAirportModal").modal('show');  
    });
    $('#addAirportModal').on('hidden.bs.modal', function () {
        $('#addAirportForm')[0].reset();
    });

    $('.close, .btn-secondary').click(function () {
        $('#addAirportModal').modal('hide');
    });
    // Handle form submission
    $("#addAirportForm").submit(function (event) {
        event.preventDefault(); 

        var airportName = $("#AirportName").val();
        var address = $("#Address").val();

        // Check if the airport name already exists
        $.ajax({
            type: "GET",
            url: "/Airport/GetByAirportName", 
            data: { airportName: airportName },
            success: function (response) {
                if (response.exists) {
                    alert("Airport name already exists. Please choose a different name.");
                } else {
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
        airportData.AirportName = row.find(".airportName input").val();  
        airportData.Address = row.find(".airportAddress input").val();  

        $.ajax({
            type: "POST",
            url: "/Airport/UpdateAirport", 
            data: JSON.stringify(airportData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
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
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var airportData = {};
            airportData.AirportID = row.find(".airportId span").text();
            $.ajax({
                type: "POST",
                url: "/Airport/DeleteAirport",
                data: JSON.stringify(airportData),  
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.message === "Airport deleted successfully.") {
                        row.remove();
                        alert(response.message); 
                    } else {
                        alert("Failed to delete the airport. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while deleting the row.");
                }
            });
        }
    });

});
