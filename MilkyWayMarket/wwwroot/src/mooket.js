export async function query(database, query) {

    console.log(query);

    const result = database.db.exec(query).then();

    console.log(result);

    return result;
}

export async function GetListOfMethodsOfObject(object) {

    if (object == null)
        alert("object is null")

    var propertyNames = Object.getOwnPropertyNames(object);

    console.log(propertyNames);

    return propertyNames;
}