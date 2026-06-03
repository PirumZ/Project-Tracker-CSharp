console.log("script loaded");

const renameModal = new bootstrap.Modal(document.getElementById("renameModal"));
const renameInput = document.getElementById("renameInput");
const renameConfirmBtn = document.getElementById("renameConfirmBtn");

renameConfirmBtn.addEventListener("click", async () => {

    const newTitle = renameInput.value.trim();
    const errorDiv = document.getElementById("renameError");

    if (!newTitle) {
        errorDiv.textContent = "Title cannot be empty.";
        return;
    }

    errorDiv.textContent = "";

    const response = await fetch(`/api/tasks/${currentRenameTaskId}/title`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ title: newTitle })
    });

    if (!response.ok) return;

    const updated = await response.json();

    updateTaskTitleUI(currentRenameTaskId, updated.title);

    renameModal.hide();

});

document.getElementById("renameModal").addEventListener("hidden.bs.modal", () => {
    renameInput.value = "";
    document.getElementById("renameError").textContent = "";
    currentRenameTaskId = null;
});

let currentRenameTaskId = null;

async function loadTasks() {
        const response = await fetch("/api/tasks");
        const tasks = await response.json();

        const taskList = document.getElementById("taskList");
        taskList.innerHTML = "";

        tasks.forEach(task => {
            taskList.append(createTaskElement(task));
        });
}

function updateTaskTitleUI(taskId, newTitle)
{
    const li = document.querySelector(`[data-task-id="${taskId}"]`);
    if (!li) return;

    const titleSpan = li.querySelector("span");
    if (!titleSpan) return;

    titleSpan.textContent = newTitle;
}

function updateTaskStatusUI(taskId, isCompleted)
{
    const li = document.querySelector(`[data-task-id="${taskId}"]`);
    if (!li) return;

    const spans = li.querySelectorAll("span");
    if (spans.length < 2) return;

    const statusSpan = spans[1]; 

    statusSpan.textContent = isCompleted ? "✅" : "❌";
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


async function renameTask(taskId)
{
    console.log("renameTask called:", taskId);


    const response = await fetch(`/api/tasks/${taskId}/title`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ title: newTitle })
    });

    console.log("fetch done.");

    if (!response.ok) return;

    const updated = await response.json();


    console.log("updateTaskTileUI about to  run");
    updateTaskTitleUI(taskId, updated.title);
}

async function toggleTask(taskId) {
    const response = await fetch(`/api/tasks/${taskId}/toggle`, {
        method: "PATCH"
    });

    if (!response.ok) {
        console.error("Failed to toggle task");
        return;
    }

    const updatedTask = await response.json();

    updateTaskStatusUI(taskId, updatedTask.isCompleted);
}

function createTaskElement(task)
{
    const li = document.createElement("li");

    li.dataset.taskId = task.id;
    li.className = "list-group-item d-flex justify-content-between align-items-center mb-2";

    const left = document.createElement("div");
    left.className = "d-flex align-items-center gap-3";

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
    deleteItem.innerHTML = `<a class="dropdown-item text-danger">Delete</a>`;
    deleteItem.querySelector("a").addEventListener("click", async (e) => {
        e.preventDefault();
        e.stopPropagation();

        const response = await fetch(`/api/tasks/${task.id}`, {
            method: "DELETE"
        });

        if (!response.ok) return;

        const row = document.querySelector(`[data-task-id="${task.id}"]`);
        row?.remove();
    });

    const toggleItem = document.createElement("li");
    toggleItem.innerHTML = `<a class="dropdown-item text-secondary">Toggle</a>`;
    toggleItem.querySelector("a").addEventListener("click", async () => {
        await toggleTask(task.id);
    });

    const renameItem = document.createElement("li");
    renameItem.innerHTML = `<a class="dropdown-item text-success">Rename</a>`;
    renameItem.querySelector("a").addEventListener("click", async (e) => {
        e.preventDefault();
        e.stopPropagation();

        currentRenameTaskId = task.id;
        renameInput.value = task.title;
        console.log("rename clicked");

        renameModal.show();
    });

    menu.append(renameItem, toggleItem, deleteItem);
    dropdownDiv.append(button, menu);

    left.append(titleSpan, statusSpan);
    li.append(left, dropdownDiv);

    return li;
}


// Listener stuff
document.addEventListener("DOMContentLoaded", () => {

    document.getElementById("loadBtn").
        addEventListener("click", loadTasks);

    document.getElementById("taskForm").
        addEventListener("submit", addTask);
});
