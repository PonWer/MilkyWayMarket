export async function query(database, query, dotNetHelper)
{
    const result = await database.db.exec(query).then();
    dotNetHelper.invokeMethodAsync('ReceiveDataFromQuery', JSON.stringify(result[0], null, 2)).then();
}



export async function GetColums(queryResult) {

    if (queryResult == null) {
        console.log("queryResult == null");
        return;
    }

    return queryResult.Colums;
}