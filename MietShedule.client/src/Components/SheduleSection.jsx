import SheduleCouple from "./SheduleCouple";
import "./SheduleSection.css"
import { useCallback, useEffect, useState } from "react";


export default function SheduleSection() {

    const url = new URL(window.location.href);
    const urlParams = new URLSearchParams(url.search)
    for (let param of urlParams.entries()) {
        console.log(`${param[0]}: ${param[1]}`);
    }
    const defaultGroup = urlParams.get("group") ?? ""
    const defaultIgnored = urlParams.get("ignore") ?? ""

    const [group, setGroup] = useState(defaultGroup)
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
    const [date, setDate] = useState((new Date()).toISOString().split('T')[0])
    const [ignored, setIgnored] = useState(defaultIgnored)

    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    const fetchShedule = useCallback(async () => {
        if (group.length != 0) {
            let query = `Shedule/${group}?dateString=${(new Date(date)).toLocaleDateString("en-GB")}`
            if (ignored.length != 0) query += `&ignored=${ignored}`
            console.log(ignored)
            const sheduleResponse = await fetch(query)
            //console.log(sheduleResponse)
            if (sheduleResponse.status == 200) {
                const sheduleRecords = await sheduleResponse.json()
                setShedule(sheduleRecords)
            }
        }
    }, [group, date, ignored])

    useEffect(() => {
        //console.log("fetch groups list")
        fetchGroupsList()
    }, [])

    useEffect(() => {
        //console.log(`effect date=${(new Date(date)).toLocaleDateString("en-GB")} group=${group}`)
        if (groupsList != undefined && groupsList.includes(group)) {
            //console.log("fetch shedule")
            fetchShedule()
        }
        else {
            //console.log("clear shedule")
            setShedule(undefined)
        }
    }, [group, date, groupsList, ignored])

    function keyExtractor(couple) {
        return `${couple.name}-${couple.date}-${couple.teacher}-${couple.order}-${couple.auditorium}-${couple.group}`;
    }

    return (
        <section className="shedule_section">
            {groupsList != undefined &&
                <>
                    <div>
                        <input type="text" list="groups" value={group} onChange={(event) => setGroup(event.target.value)} />
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    </div>
                    <div>
                        <input type="date"
                            value={date}
                            onChange={(event) => setDate(event.target.value)}>
                        </input>
                    </div>
                </>

            }

            {shedule != undefined &&
                shedule.map(c =>
                    <SheduleCouple key={keyExtractor(c)} {...c}></SheduleCouple>
                )
            }

            {
                shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }

            {
                ignored.length != 0 &&
                <p>Ignore: {ignored}</p>
            }
        </section>
    )
}