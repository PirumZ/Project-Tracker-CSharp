console.log("script loaded");

    async function loadTasks() {
            const response = await fetch("/api/tasks");
    const tasks = await response.json();

    const taskList = document.getElementById("taskList");

    taskList.innerHTML = "";

            tasks.forEach(task => {


                const li = document.createElement("li");

    li.dataset.taskID = task.id;

    li.className = "list-group-item d-flex justify-content-between align-items-center";

    const left = document.createElement("div");
    left.className = "d-flex align-items-center gap-2";

    const titleSpan = document.createElement("span");

    titleSpan.textContent = task.title;

    const statusSpan = document.createElement("span");

    statusSpan.textContent = task.isCompleted ? "✅" : "❌";

    const dropdownDiv = document.createElement("div");

    dropdownDiv.className = "dropdown";

    const button = document.createElement("button");

    button.className = "btn btn-sm btn-secondary";
    button.setAttribute("data-bs-toggle", "dropdown");
    button.textContent = "⋮";

    const menu = document.createElement("ul");

    menu.className = "dropdown-menu";

    const deleteItem = document.createElement("li");
    deleteItem.innerHTML =
    `<a class="dropdown-item text-danger">Delete<a />`;

                deleteItem.addEventListener("click", async () => {

            await fetch(`/api/tasks/${task.id}`, {
                method: "DELETE"
            });

        loadTasks();
                });


        const toggleItem = document.createElement("li");
        toggleItem.innerHTML =
        `<a class="dropdown-item text-secondary">Toggle</a>`;

                toggleItem.addEventListener("click", async () => {

            await fetch(`/api/tasks/${task.id}`, {
                method: "PUT",
                headers:
                {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    title: task.title,
                    isCompleted: !task.isCompleted
                })
            });

        loadTasks();

                });



        const updateItem = document.createElement("li");
        updateItem.innerHTML =
        `<a class="dropdown-item text-warning">Update<a />`;

                updateItem.addEventListener("click", async () => {

                    const newTitle = prompt("New title:");

            if (!newTitle) { return; }

            await fetch(`/api/tasks/${task.id}`, {
                        method: "PUT",
                        headers:
                        {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            title: newTitle,
                            isCompleted: task.isCompleted
                        })
                    });

                    loadTasks();

                });

                menu.append(deleteItem);
                menu.append(toggleItem);
                menu.append(updateItem);

                dropdownDiv.append(button);
                dropdownDiv.append(menu);

                left.append(titleSpan);
                left.append(statusSpan);
                
                li.append(left);
                li.append(dropdownDiv);

                taskList.append(li);
            }
            );
        }

        async function addTask(event) {
            event.preventDefault();

            const titleInput = document.getElementById("taskName");

    
            const task = {
                title: titleInput.value,
            };

            const response = await fetch("/api/tasks", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(task)
            });

            if (!response.ok)
            {
                const errorData = await response.json();

                console.log(errorData);

                const errorDiv = document.getElementById("errorMessage");

                const message =
                    errorData?.errors?.Title?.[0] ??
                    errorData?.Title?.[0] ??
                    "Invalid input";

                errorDiv.textContent = message;

                setTimeout(() => {
                    errorDiv.textContent = "";
                }, 2000);

                return;
            }

            if (response.ok) { 

                titleInput.value = "";
                loadTasks();
            }
            
        }


// Listener stuff
document.addEventListener("DOMContentLoaded", () => {

    document.getElementById("loadBtn").
        addEventListener("click", loadTasks);

    document.getElementById("taskForm").
        addEventListener("submit", addTask);
});
