$(document).ready(function () {
    $("#openAddPlaneModal").click(function (e) {
        e.preventDefault();  
        $("#addPlaneModal").modal('show');  
    });
    $('#addPlaneModal').on('hidden.bs.modal', function () {
        $('#addPlaneForm')[0].reset();
    });
    $('.close, .btn-secondary').click(function () {
        $('#addPlaneModal').modal('hide');
    });

    // Handle form submission
    $("#addPlaneForm").submit(function (event) {
        event.preventDefault(); 

        var code = $("#PlaneCode").val();
        var airline = $("#PlaneAirline").val();
        var model = $("#PlaneModel").val();

        var planeData = {
            PlaneCode: code,
            Airline: airline,
            Model:model
        };

        $.ajax({
            type: "POST",
            url: "/Plane/AddPlane",
            data: JSON.stringify(planeData),
            contentType: "application/json",
            success: function (response) {
                if (response && response.message) {
                    alert(response.message);
                    $("#addPlaneModal").modal('hide');
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
        var planeData = {};
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

        planeData.planeId = row.find(".planeId span").text();
        planeData.planeCode = row.find(".planeCode input").val(); 
        planeData.planeAirline = row.find(".planeAirline input").val(); 
        planeData.planeModel = row.find(".planeModel input").val();  

        $.ajax({
            type: "POST",
            url: "/Plane/UpdatePlane",
            data: JSON.stringify(planeData),
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
    $("body").on("click", "#tblPlane .delete", function () {
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            var planeData = {};
            planeData.PlaneID = row.find(".planeId span").text();
            $.ajax({
                type: "POST",
                url: "/Plane/DeletePlane",
                data: JSON.stringify(planeData),   
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    if (response.message === "Plane deleted successfully.") {
                        
                        row.remove();

                        alert(response.message);
                    } else {
                        alert("Failed to delete the plane. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                   
                    alert("An error occurred while deleting the row.");
                }
            });
        }
    });

});
