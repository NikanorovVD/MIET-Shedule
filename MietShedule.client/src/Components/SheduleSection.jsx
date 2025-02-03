import SheduleCouple from "./SheduleCouple";
import "./SheduleSection.css"
import { useCallback, useEffect, useState } from "react";


export default function SheduleSection() {
    const [group, setGroup] = useState("")
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
    const [date, setDate] = useState((new Date()).toISOString().split('T')[0])

    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    const fetchShedule = useCallback(async () => {
        if (group.length != 0) {
            const sheduleResponse = await fetch(`Shedule/${group}?dateString=${(new Date(date)).toLocaleDateString("en-GB")}`)
            console.log(sheduleResponse)
            if (sheduleResponse.status == 200) {
                const sheduleRecords = await sheduleResponse.json()
                setShedule(sheduleRecords)
            }
        }
    }, [group, date])

    useEffect(() => {
        fetchGroupsList()
    }, [])

    useEffect(() => {
        console.log(`effect date=${(new Date(date)).toLocaleDateString("en-GB")} group=${group}`)
        if (groupsList != undefined && groupsList.includes(group)) {
            fetchShedule()
        }
    }, [group, date])


    return (
        <section>
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
                    <SheduleCouple key={c.order} {...c}></SheduleCouple>
                )
            }

        </section>
    )
}