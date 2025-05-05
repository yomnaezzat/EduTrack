$(document).ready(function () {
    // Get references to the search elements
    const searchInput = $('#searchInput');
    const searchResults = $('#searchResults');
    const searchForm = $('#searchForm');

    // Add event listeners
    searchInput.on('input', debounce(handleSearch, 500));
    searchForm.on('submit', function (e) {
        e.preventDefault();
        handleSearch();
    });

    // Function to handle search requests
    function handleSearch() {
        const searchTerm = searchInput.val().trim();
        
        // Clear results if search term is empty
        if (!searchTerm) {
            searchResults.empty();
            return;
        }
        
        // Make API request
        $.ajax({
            url: `/api/CoursesApi/search?term=${encodeURIComponent(searchTerm)}`,
            method: 'GET',
            success: function (data) {
                displayResults(data);
            },
            error: function (error) {
                console.error('Error searching courses:', error);
                searchResults.html('<div class="alert alert-danger">An error occurred while searching. Please try again.</div>');
            }
        });
    }
    
    // Function to display search results
    function displayResults(courses) {
        searchResults.empty();
        
        if (courses.length === 0) {
            searchResults.html('<div class="alert alert-info">No courses found.</div>');
            return;
        }
        
        const resultsHtml = courses.map(course => `
            <div class="col-md-4 mb-4">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">${escapeHtml(course.title)}</h5>
                        <h6 class="card-subtitle mb-2 text-muted">$${course.price.toFixed(2)}</h6>
                        <p class="card-text">
                            ${course.description ? escapeHtml(truncateText(course.description, 100)) : 'No description available.'}
                        </p>
                        <p class="card-text">
                            <small class="text-muted">Material Type: ${escapeHtml(course.materialType || 'Not specified')}</small>
                        </p>
                    </div>
                    <div class="card-footer bg-transparent">
                        <a href="/Courses/Details/${course.id}" class="btn btn-sm btn-primary">View Details</a>
                    </div>
                </div>
            </div>
        `).join('');
        
        searchResults.html(`<div class="row">${resultsHtml}</div>`);
    }
    
    // Helper function to debounce input events
    function debounce(func, wait) {
        let timeout;
        return function () {
            const context = this, args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                func.apply(context, args);
            }, wait);
        };
    }
    
    // Helper function to truncate text
    function truncateText(text, maxLength) {
        if (!text) return '';
        return text.length > maxLength ? text.substring(0, maxLength) + '...' : text;
    }
    
    // Helper function to escape HTML
    function escapeHtml(unsafe) {
        if (!unsafe) return '';
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
});
